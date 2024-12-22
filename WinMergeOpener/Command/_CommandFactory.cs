using System;
using System.Collections.Generic;
using System.Linq;

namespace WinMergeOpener.Command
{
    public class CommandFactory
    {
        readonly CommandBase[] Commands;

        public CommandFactory(VersionManager versionManager)
        {
            var smdTypes = typeof(CommandBase).Assembly.GetTypes().Where(n => n.IsSubclassOf(typeof(CommandBase)) && n != typeof(CommandBase));

            Commands = smdTypes.Select(n => (Activator.CreateInstance(n) as CommandBase).SetManager(versionManager, this)).ToArray();
        }

        public CommandBase FindCommand(ConsoleParam consoleParam)
        {
            return Commands.FirstOrDefault(n => n.IsRun(consoleParam));
        }

        public string GetMenus()
        {
            return string.Join("\r\n", Commands.Select(n => n.GetMenuName()));
        }

    }

    public abstract class CommandBase
    {
        protected CommandFactory _CommandFactory;
        protected VersionManager _VersionManager;
        public abstract void Run(ArgumentParam ArgParam, ConsoleParam ConsoleParam);
        public abstract bool IsRun(ConsoleParam ConsoleParam);
        public CommandBase SetManager(VersionManager versionManager, CommandFactory commandFactory)
        {
            _CommandFactory = commandFactory;
            _VersionManager = versionManager;
            return this;
        }

        public virtual string GetMenuName()
        {
            return string.Format("Menu Tanımı Yok :{0}", this.GetType().Name);
        }

    }
}
