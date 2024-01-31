import React from 'react'
import {$Panel, useDataUpdate} from 'hookui-framework'

const $SettingsPage = ({react, data}) => {
    const updateData = (field, val) => {
        engine.trigger('tree-wind-controller.set_value', field, val)
    }

    return <div>
        <$Slider react={react} value={data["wind_strength"]} label="Wind Strength:" onValueChanged={(val) => updateData("wind_strength", val)} />
        <$Slider react={react} value={data["wind_direction"]} label="Wind Direction:" max={360} unit="&deg;" onValueChanged={(val) => updateData("wind_direction", val)} />
        <$Slider react={react} value={5} label="Wind Period:" min={5} max={60} unit="s" onValueChanged={(val) => updateData("wind_period", val)} />
        <$Slider react={react} value={30} label="Wind Variance:" onValueChanged={(val) => updateData("wind_variance", val)} />
    </div>

}

const $TreeWindController = ({react}) => {
    const [data, setData] = react.useState(0)
    useDataUpdate(react, "tree-wind-controller.get_values", setData)

    const style = {
        height: "auto",
    }

    const size = {
        width: 700,
        height: 800, // 200 * number of panels :|
    }

    const toggleVisibility = () => {
        const visData = { type: "toggle_visibility", id: "tree-wind-controller" };
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
    id: "tree-wind-controller",
    name: "Tree Wind Controller",
    // TODO find a better icon
    icon: "Media/Game/Icons/Trees.svg",
    component: $TreeWindController
})


// TODO move to separate file
const $Slider = ({ react, value, onValueChanged, style, label, min, max, unit }) => {
    const sliderRef = react.useRef(null);
    const [isMouseDown, setIsMouseDown] = react.useState(false);

    min = min ?? 0;
    max = max ?? 100;
    unit = unit ?? "%";

    const updateValue = (e) => {
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
        setIsMouseDown(true);
        updateValue(e);
        engine.trigger("audio.playSound", "grabSlider", 1);
    };

    const handleMouseMove = (e) => {
        if (isMouseDown) {
            updateValue(e);
        }
    };

    const handleMouseUp = () => {
        setIsMouseDown(false);
    };

    const displayValue = value + unit;
    const valuePercent = 100 * (value - min) / (max - min);
    return (
        <div style={{ width: '100%', ...style }}>
            <div style={{ marginLeft: '10rem', marginTop: '10rem' }}>
                {label}
            </div>
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center', margin: '10rem', marginTop: '0' }}>
                <div className="value_jjh" style={{ display: 'flex', width: '45rem', alignItems: 'center', justifyContent: 'center' }}>{displayValue}</div>
                <div
                    className="slider_fKm slider_pUS horizontal slider_KjX"
                    style={{ flex: 1, margin: '10rem' }}
                    ref={sliderRef}
                    onMouseDown={handleMouseDown}
                    onMouseMove={handleMouseMove}
                    onMouseUp={handleMouseUp}>
                    <div className="track-bounds_H8_">
                        <div className="range-bounds_lNt" style={{ width: valuePercent + "%" }}>
                            <div className="range_KXa range_iUN"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}