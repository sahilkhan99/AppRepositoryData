﻿using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeV2.ViewModel;
using Foundation;
using UIKit;
using XLabs.Ioc;
using XLabs.Platform.Device;

namespace EmployeeV2.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            if (!Resolver.IsSet)
                SetIoc();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        private void SetIoc()
        {
            var resolverContainer = new SimpleContainer();

            resolverContainer.Register<IDevice>(r => AppleDevice.CurrentDevice);
            resolverContainer.Register<MainViewModel>(r => new MainViewModel(r.Resolve<IDevice>()));

            Resolver.SetResolver(resolverContainer.GetResolver());
        }
    }
}
