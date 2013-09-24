﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using NullGuard;
using RomanticWeb.Entities;
using RomanticWeb.Ontologies;

namespace RomanticWeb
{
    internal class QuadCollection
    {
        private readonly IDictionary<RdfNode,ISet<Triple>> _quads;

        public QuadCollection()
        {
            _quads=new ConcurrentDictionary<RdfNode,ISet<Triple>>();
        }

        public IEnumerable<Triple> Triples
        {
            get
            {
                return _quads.SelectMany(e => e.Value);
            }
        }

        public void AssertQuad([AllowNull] RdfNode graph,Triple triple)
        {
            if (!_quads.ContainsKey(graph))
            {
                _quads[graph]=new HashSet<Triple>();
            }

            _quads[graph].Add(triple);
        }
    }
}