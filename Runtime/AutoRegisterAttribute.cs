using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace Popcron.CommandRunner
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    [Preserve]
    public class AutoRegisterAttribute : PropertyAttribute
    {

    }
}