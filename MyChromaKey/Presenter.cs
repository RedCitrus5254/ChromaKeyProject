using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChromaKey
{
    class Presenter : ISubscriber
    {
        private IView view;
        private IChromaKey chromaKey;
        public Presenter(IView view)
        {
            this.view = view;
            chromaKey = new ChromaKey();
            chromaKey.Subscribe(this);
        }

        public void CropImage(Image inputImage )
        {
            chromaKey.CropImage(inputImage);
        }

        public void UpdateImage(Bitmap outputImage)
        {
            view.SetImage(outputImage);
        }
    }
}
