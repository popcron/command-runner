using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Popcron.CommandRunner.Tests
{
    public class Tests
    {
        [TearDown]
        private void TearDown()
        {
            Debug.Log("tear down");
        }

        [SetUp]
        private void SetUp()
        {
            Debug.Log("setup");
        }

        [Test]
        public void NewTestScriptSimplePasses()
        {
            Debug.Log("test");
        }
    }
}