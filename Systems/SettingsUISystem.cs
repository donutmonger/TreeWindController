using System;
using System.Collections.Generic;
using System.Text;
using Game.UI;
using Colossal.Annotations;
using Colossal.UI.Binding;

namespace TreeWindController.Systems {

    class SettingsUISystem : UISystemBase {
        private string kGroup = "tree-wind-controller";
        protected override void OnCreate() {
            base.OnCreate();

            this.AddUpdateBinding(new GetterValueBinding<float>(this.kGroup, "wind_strength", () => {
                var strength = SettingsSystem.Instance.strength;

                return 100f * (strength.value - strength.min) / (strength.max - strength.min);
            }));

            this.AddBinding(new TriggerBinding<float>(kGroup, "set_wind_strength", new Action<float>(SetWindStrengthPercent)));
        }


        private void SetWindStrengthPercent(float strengthPercent) {
            var strength = SettingsSystem.Instance.strength;
            SettingsSystem.Instance.strength.value = strength.min + strengthPercent/100f * (strength.max - strength.min);
        }
    }
}
