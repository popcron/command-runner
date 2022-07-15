using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace Popcron.CommandRunner
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property)]
    [Preserve]
    public class CommandAttribute : PropertyAttribute, ICommand
    {
        private MemberInfo member;

        public string Path { get; }

        public CommandAttribute(string path)
        {
            Path = path;
        }

        public void Set(MemberInfo member)
        {
            this.member = member;
        }

        void ICommand.Run(Context context)
        {
            if (member is MethodBase method)
            {
                method.Invoke(null, null);
            }
        }
    }
}