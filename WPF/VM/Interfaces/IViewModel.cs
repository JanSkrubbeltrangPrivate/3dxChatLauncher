using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.ViewModels;

namespace VM.Interfaces
{
    public interface IViewModel
    {
        IViewModel? Parent { get; }
        AppViewModel App { get; }
    }
}
