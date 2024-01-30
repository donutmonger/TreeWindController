using System;
using System.Collections.Generic;
using System.Text;
using Game.UI;
using Colossal.Annotations;

namespace TreeWindController.Systems {

    class SettingsUISystem : UISystemBase {
        private string kGroup = "tree-wind-controller";
        protected override void OnCreate() {
            base.OnCreate();

            //this.AddUpdateBinding(new GetterValueBinding<ImmutableDictionary<string, MeterSetting>>(kGroup, "meters", () => {
            //    return meters;
            //}, new MyDictionaryWriter<string, MeterSetting>()));


            //this.AddBinding(new TriggerBinding<string, bool>(kGroup, "toggle_visibility", new Action<string, bool>(ToggleVisibility)));
        }
    }
}
