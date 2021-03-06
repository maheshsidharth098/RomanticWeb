﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using FluentAssertions;
using ImpromptuInterface;
using Moq;
using NUnit.Framework;
using RomanticWeb.Collections;
using RomanticWeb.Dynamic;
using RomanticWeb.Entities;
using RomanticWeb.Entities.ResultPostprocessing;
using RomanticWeb.Mapping.Model;
using RomanticWeb.Model;
using RomanticWeb.TestEntities;

namespace RomanticWeb.Tests.Collections
{
    [TestFixture]
    public class DictionaryTransformerTests
    {
        private static readonly Uri Id = new Uri("urn:parent:id");
        private DictionaryTransformer _transformer;
        private Mock<IEntityContext> _context;
        private Mock<IDictionaryTypeProvider> _provider;
        private Owner _owner;
        private IPropertyMapping _property;

        [SetUp]
        public void Setup()
        {
            _owner = new Owner(Id);
            _property = CreateDictionaryProperty();
            _context = new Mock<IEntityContext>(MockBehavior.Strict);
            _context.Setup(c => c.Load<Owner>(Id)).Returns(_owner);
            _provider = new Mock<IDictionaryTypeProvider>(MockBehavior.Strict);
            _provider.Setup(p => p.GetEntryType(_property)).Returns(typeof(DictionaryPair));
            _provider.Setup(p => p.GetOwnerType(_property)).Returns(typeof(Owner));
            _transformer = new DictionaryTransformer(_provider.Object);
        }

        [TearDown]
        public void Teardown()
        {
        }

        [Test]
        public void Should_convert_collection_of_entities_to_RdfDictionary()
        {
            // given
            var proxy = CreateProxy();
            object elements = CreateDictionaryEntries();

            // when
            var dict = _transformer.FromNodes(proxy, _property, _context.Object, new Node[0]);

            // then
            dict.Should().BeOfType<RdfDictionary<int, IPerson, DictionaryPair, Owner>>();
        }

        [Test]
        public void Should_retrieve_the_type_of_dictionary_parent()
        {
            // given
            var proxy = CreateProxy();
            object elements = CreateDictionaryEntries();

            // when
            _transformer.FromNodes(proxy, _property, _context.Object, new Node[0]);

            // then
            _provider.Verify(p => p.GetEntryType(_property));
        }

        private IEnumerable<IEntity> CreateDictionaryEntries()
        {
            return from index in Enumerable.Range(1, 5)
                   select new
                              {
                                  Id = new BlankId(string.Format("test{0}", index))
                              }.ActLike<IEntity>();
        }

        private IPropertyMapping CreateDictionaryProperty()
        {
            return new
                       {
                           ReturnType = typeof(IDictionary<int, IPerson>)
                       }.ActLike<IPropertyMapping>();
        }

        private IEntityProxy CreateProxy()
        {
            dynamic expando = new ExpandoObject();
            expando.Id = Id;
            return Impromptu.ActLike<IEntityProxy>(expando);
        }

        private class DictionaryPair : IDictionaryEntry<int, IPerson>
        {
            private readonly EntityId _id;

            public DictionaryPair(EntityId id)
            {
                _id = id;
            }

            public EntityId Id
            {
                get
                {
                    return _id;
                }
            }

            public IEntityContext Context { get; private set; }

            public int Key { get; set; }

            public IPerson Value { get; set; }
        }

        private class Owner : IDictionaryOwner<DictionaryPair, int, IPerson>
        {
            public Owner(EntityId id)
            {
                Id = id;
            }

            public EntityId Id { get; private set; }

            public IEntityContext Context { get; private set; }

            public ICollection<DictionaryPair> DictionaryEntries { get; set; }
        }
    }
}