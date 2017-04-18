 /**
  * Functions and logic to get data from a currently playing source, by setting up an AnalyserNode to watch
  * a source in the Web Audio API. This will not work in browsers that do not use the Web Audio API (IE).
  */
 var SpectrumDataWA = {
     $analyzers: {},

     StartSampling: function(namePtr, duration, bufferSize) {
         var acceptableDisantce = 0.075;

         var name = Pointer_stringify(namePtr);
         if (analyzers[name] != null) return;

         var analyzer = null;
         var source = null;

         try {
             if (typeof WEBAudio != 'undefined' && WEBAudio.audioInstances.length > 1) {
                 for (var i = WEBAudio.audioInstances.length - 1; i >= 0; i--) {
                     if (WEBAudio.audioInstances[i] != null) {
                         var pSource = WEBAudio.audioInstances[i].source;
                         if (pSource != null && pSource.buffer != null && Math.abs(pSource.buffer.duration - duration) < acceptableDisantce) {
                             source = pSource;
                             break;
                         }
                     }
                 }

                 if (source == null) {
                     return false;
                 }

                 analyzer = source.context.createAnalyser();
                 analyzer.fftSize = bufferSize * 2;
                 source.connect(analyzer);

                 analyzers[name] = {
                     analyzer: analyzer,
                     source: source
                 };

                 return true;
             }
         } catch (e) {
             console.log("Failed to connect analyser to source " + e);

             if (analyzer != null && source != null) {
                 source.disconnect(analyzer);
             }
         }

         return false;
     },

     CloseSampling: function(namePtr) {
         var name = Pointer_stringify(namePtr);
         var analyzerObj = analyzers[name];

         if (analyzerObj != null) {
             try {
                 analyzerObj.source.disconnect(analyzerObj.analyzer);
                 delete analyzers[name];
                 return true;
             } catch (e) {
                 console.log("Failed to disconnect analyser " + name + " from source " + e);
             }
         }

         return false;
     },

     GetSamples: function(namePtr, bufferPtr, bufferSize) {
         var name = Pointer_stringify(namePtr);
         if (analyzers[name] == null) return;
         try {
             var buffer = new Uint8Array(Module.HEAPU8.buffer, bufferPtr, Float32Array.BYTES_PER_ELEMENT * bufferSize);
             buffer = new Float32Array(buffer.buffer, buffer.byteOffset, bufferSize);

             var analyzerObj = analyzers[name];

             if (analyzerObj == null) {
                 console.log("Could not find analyzer " + name + " to get lipsync data for");
                 return false;
             }

             analyzerObj.analyzer.getFloatTimeDomainData(buffer);
             for (var i = 0; i < buffer.length; i++) {
                 buffer[i] /= 4;
             }
             return true;
         } catch (e) {
             console.log("Failed to get lipsync sample data " + e);
         }

         return false;
     }
 };

 autoAddDeps(SpectrumDataWA, '$analyzers');
 mergeInto(LibraryManager.library, SpectrumDataWA);