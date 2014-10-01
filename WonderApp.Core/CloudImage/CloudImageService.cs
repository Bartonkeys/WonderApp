﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WonderApp.Core.CloudImage
{
    public class CloudImageService
    {
        protected ICloudImageProvider CloudProvider;


        public CloudImageService(ICloudImageProvider cloudProvider)
        {
            CloudProvider = cloudProvider;
        }

        public string SaveImageToCloud(Image image, string imageName)
        {
            return CloudProvider.SaveImageToCloud(image, imageName);
        }

        public string SaveImageToCloud(string imagePath, string imageName)
        {
            return CloudProvider.SaveImageToCloud(imagePath, imageName);
        }

    }
}
