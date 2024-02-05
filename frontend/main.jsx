import React from 'react'
import {$Panel, useDataUpdate} from 'hookui-framework'
import {$CheckBox} from './checkbox'
import {$Slider} from './slider'

const panelID = "tree-wind-controller";

const $SettingsPage = ({react, data}) => {
    const setBoolValue = (field, val) => {
        engine.trigger(panelID + ".set_bool_value", field, val);
    }
    const setFloatValue = (field, val) => {
        engine.trigger(panelID + ".set_value", field, val);
    }

    const slidersDisabled = data["disable_wind"]?.checkedValue;

    let keys = Object.keys(data)
    const meters = keys.map((k) => {
        const { __Type } = data[k]

        switch (__Type) {
            case "TreeWindController.UI.Checkbox":
                var { label, checkedValue } = data[k]
                return <$CheckBox react={react} label={label} checked={checkedValue} onToggle={(val) => setBoolValue(k, val)} />
                break;

            case "TreeWindController.UI.Slider":
                var { label, min, max, value, unit } = data[k]
                return < $Slider react={react} value={value} label={label} min={min} max={max} unit={unit} disabled={slidersDisabled} onValueChanged={(val) => setFloatValue(k, val)} />
                break;
        }

    })

    return <div>
        {...meters }
    </div>

}

const $TreeWindController = ({react}) => {
    const [data, setData] = react.useState(0)
    useDataUpdate(react, panelID+".get_values", setData)

    const style = {
        height: "auto",
    }

    // TODO make this dynamic
    const numSliders = 6;
    const numCheckboxes = 1;

    const size = {
        width: 700,
        height: 200 * numSliders + 40 * numCheckboxes, 
    }

    const toggleVisibility = () => {
        const visData = { type: "toggle_visibility", id: panelID };
        const event = new CustomEvent('hookui', { detail: visData });
        window.dispatchEvent(event);

        engine.trigger("audio.playSound", "close-panel", 1);
    }

    return <div>
        <$Panel title="Tree Wind Controller" react={react} style={style} initialSize={size} onClose={toggleVisibility}>
            <$SettingsPage react={react} data={data}/>
        </$Panel>
    </div>
}

window._$hookui.registerPanel({
    id: panelID,
    name: "Tree Wind Controller",
    icon: "coui://" + panelID + "/wind.svg",
    component: $TreeWindController
})



