using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace Signals.Contracts
{
    public interface IFilePicker
    {
        public Task<StorageFile> ShowSaveDialogAsync(params KeyValuePair<string, IList<string>>[] types);

        public Task<StorageFile> ShowOpenDialogAsync(params string[] extensions);

    }
}
