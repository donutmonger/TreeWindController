import React from 'react'
import {$Panel, useDataUpdate} from 'hookui-framework'

const panelID = "tree-wind-controller";

const $SettingsPage = ({react, data}) => {
    const setFloatValue = (field, val) => {
        engine.trigger(panelID + ".set_value", field, val);
    }
    const setBoolValue = (field, val) => {
        engine.trigger(panelID + ".set_bool_value", field, val);
    }

    //console.log(JSON.stringify(data["disable_wind"]));

    const slidersDisabled = data["disable_wind"]?.checkedValue;

    let keys = Object.keys(data)
    const meters = keys.map((k) => {
        // TODO check "__Type" field to determine which element to render
        const { __Type } = data[k]

        switch (__Type) {
            case "TreeWindController.Systems.Checkbox":
                var { label, checkedValue } = data[k]
                return <$CheckBox react={react} label={label} checked={checkedValue} onToggle={(val) => setBoolValue(k, val)} />
                break;

            case "TreeWindController.Systems.Slider":
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
    const numSliders = 5;
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
    // TODO find a better icon, maybe https://windicss.org/assets/logo.svg?
    icon: "Media/Game/Icons/Trees.svg",
    component: $TreeWindController
})


// TODO move to separate file
// Modified from https://github.com/Cities2Modding/LegacyFlavour/blob/25b5fbfe547a5b58415e98ce46149fb963b49256/LegacyFlavour.Frontend/src/jsx/components/_slider.jsx
const $Slider = ({ react, value, onValueChanged, style, label, min, max, unit, disabled }) => {
    const sliderRef = react.useRef(null);
    const [isMouseDown, setIsMouseDown] = react.useState(false);

    min = min ?? 0;
    max = max ?? 100;
    unit = unit ?? "%";

    const updateValue = (e) => {
        if (disabled) {
            return;
        }

        const slider = sliderRef.current;
        if (!slider) return;

        const rect = slider.getBoundingClientRect();
        const position = e.clientX - rect.left;
        let newValue = (position / rect.width) * max;
        newValue = Math.max(min, Math.min(max, Math.round(newValue)));

        if (onValueChanged)
            onValueChanged(newValue);

        engine.trigger("audio.playSound", "drag-slider", 1);
    };

    const handleMouseDown = (e) => {
        if (disabled) {
            return;
        }

        setIsMouseDown(true);
        updateValue(e);
        engine.trigger("audio.playSound", "grabSlider", 1);
    };

    const handleMouseMove = (e) => {
        if (disabled) {
            return;
        }

        if (isMouseDown) {
            updateValue(e);
        }
    };

    const handleMouseUp = () => {
        if (disabled) {
            return;
        }

        setIsMouseDown(false);
    };

    const displayValue = Math.round(value) + unit;
    const valuePercent = 100 * (value - min) / (max - min);
    const displayLabel = label + ":";
    const disabledColor = disabled ? '#8f8f8f' : undefined;

    return (
        <div style={{ width: '100%', ...style }}>
            <div style={{ marginLeft: '10rem', marginTop: '10rem', color: disabledColor }}>
                {displayLabel}
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center', margin: '10rem', marginTop: '0' }}>
                <div className="value_jjh" style={{ display: 'flex', width: '45rem', alignItems: 'center', justifyContent: 'center' }}>{displayValue}</div>
                <div
                    className="slider_fKm slider_pUS horizontal slider_KjX"
                    style={{ flex: 1, margin: '10rem', borderColor: disabledColor }}
                    ref={sliderRef}
                    onMouseDown={handleMouseDown}
                    onMouseMove={handleMouseMove}
                    onMouseUp={handleMouseUp}>
                    <div className="track-bounds_H8_">
                        <div className="range-bounds_lNt" style={{ width: valuePercent + "%" }}>
                            <div className="range_KXa range_iUN" style={{ backgroundColor: disabledColor }}></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

// TODO move to separate file
// Modified from https://github.com/Cities2Modding/LegacyFlavour/blob/25b5fbfe547a5b58415e98ce46149fb963b49256/LegacyFlavour.Frontend/src/jsx/components/_checkbox.jsx
const $CheckBox = ({ react, style, checked, label, onToggle }) => {
    const [isChecked, setIsChecked] = react.useState(checked);

    const handleClick = () => {
        onToggle(!checked)
        engine.trigger("audio.playSound", "select-toggle", 1);
    }

    react.useEffect(() => {
        setIsChecked(checked);
    }, [checked]);

    const checked_class = isChecked ? 'checked' : 'unchecked';

    const many = (...styles) => {
        return styles.join(' ')
    }

    const displayLabel = label + ":";

    return (
        <div style={{ width: '100%', ...style }}>
            <div style={{ display: 'flex', flexDirection: 'row', margin: '10rem', marginBottom: '0', justifyContent: 'space-between' }}>
                <div style={{ }}>
                    {displayLabel}
                </div>
                <div className={many('toggle_cca toggle_ATa', checked_class)} style={{ marginRight: '10rem' }} onClick={handleClick}>
                    <div className={many('checkmark_NXV', checked_class)}></div>
                </div>
            </div>
        </div>
    );
}