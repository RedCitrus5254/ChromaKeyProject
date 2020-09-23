using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChromaKey
{
    interface IChromaKey
    {
        void CropImage(Image image);

        void Subscribe(ISubscriber subscriber);
    }
}
