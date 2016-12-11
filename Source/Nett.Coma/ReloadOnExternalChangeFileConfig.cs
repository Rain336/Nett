﻿using Nett.Extensions;

namespace Nett.Coma
{
    internal sealed class ReloadOnExternalChangeFileConfig : IPersistableConfig
    {
        private readonly FileConfig persistable;

        private TomlTable loaded = null;
        private TomlTable loadedSourcesTable = null;

        public ReloadOnExternalChangeFileConfig(FileConfig persistable)
        {
            this.persistable = persistable.CheckNotNull(nameof(persistable));
        }

        public bool CanHandleSource(IConfigSource source) => this.persistable.CanHandleSource(source);

        public bool EnsureExists(TomlTable content) => this.persistable.EnsureExists(content);

        public TomlTable Load()
        {
            this.EnsureLastestTableLoaded();
            return this.loaded.Clone();
        }

        public TomlTable LoadSourcesTable()
        {
            this.EnsureLastestTableLoaded();
            return this.loadedSourcesTable.Clone();
        }

        public void Save(TomlTable content)
        {
            this.persistable.Save(content);
            this.loaded = content;
            this.loadedSourcesTable = this.persistable.TransformToSourceTable(this.loaded);
            this.loadedSourcesTable.Freeze();
        }

        public bool WasChangedExternally() => this.persistable.WasChangedExternally();

        private void EnsureLastestTableLoaded()
        {
            if (this.loaded == null || this.persistable.WasChangedExternally())
            {
                this.loaded = this.persistable.Load();
                this.loadedSourcesTable = this.persistable.TransformToSourceTable(this.loaded);

                this.loaded.Freeze();
                this.loadedSourcesTable.Freeze();
            }
        }
    }
}
