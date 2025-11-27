using Microsoft.Extensions.AI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics.Tensors;

namespace ASPxGridViewAIIntegration {
    public class SmartFilterProvider {
        private readonly IEmbeddingGenerator<string, Embedding<float>> embedder;
        private static readonly ConcurrentDictionary<string, Embedding<float>> cache = new ConcurrentDictionary<string, Embedding<float>>(StringComparer.OrdinalIgnoreCase);

        public SmartFilterProvider(IEmbeddingGenerator<string, Embedding<float>> embedder) {
            this.embedder = embedder;
        }

        public void FillCache(IEnumerable<string> words) {
            var distinct = words?.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            if(distinct == null || distinct.Length == 0) return;

            var nonCached = distinct.Where(x => !cache.ContainsKey(x)).ToArray();
            if(!nonCached.Any()) return;

            var embeddings = embedder.GenerateAsync(nonCached).GetAwaiter().GetResult();
            int i = 0;
            foreach(var emb in embeddings) {
                cache[nonCached[i]] = emb;
                i++;
            }
        }

        public float GetSimilarity(string text, string searchText) {
            if(!cache.TryGetValue(text, out var eText)) throw new InvalidOperationException($"Embedding for '{text}' not found in cache.");
            if(!cache.TryGetValue(searchText, out var eQuery)) throw new InvalidOperationException($"Embedding for query '{searchText}' not found in cache.");

            var sim = TensorPrimitives.CosineSimilarity(eQuery.Vector.Span, eText.Vector.Span);
            return sim;
        }
    }
}