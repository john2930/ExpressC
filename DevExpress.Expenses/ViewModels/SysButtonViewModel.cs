using Expenses.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Wpf {
    class SysButtonViewModel : ViewModelBase {
        private Uri normalGlyphSource;
        public Uri NormalGlyphSource {
            get { return this.normalGlyphSource; }
            set {
                if(this.normalGlyphSource == value) { return; }

                this.normalGlyphSource = value;
                this.NotifyOfPropertyChange(() => this.NormalGlyphSource);
            }
        }
        private Uri hoverGlyphSource;
        public Uri HoverGlyphSource {
            get { return this.hoverGlyphSource; }
            set {
                if(this.hoverGlyphSource == value) { return; }

                this.hoverGlyphSource = value;
                this.NotifyOfPropertyChange(() => this.HoverGlyphSource);
            }
        }
    }
}
