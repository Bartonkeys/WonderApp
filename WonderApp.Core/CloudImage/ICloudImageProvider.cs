﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WonderApp.Core.CloudImage
{
    public interface ICloudImageProvider
    {
        string SaveImageToCloud(Image image, string imageName);
        string SaveImageToCloud(string imagePath, string imageName);
    }
}
