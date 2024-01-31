import React from 'react'
import {$Meter, $Panel, useDataUpdate} from 'hookui-framework'

const $SettingsPage = ({react, data}) => {
    const updateData = (field, val) => {
        console.log("updateData called with field=" + field + ", val=" + val);
        engine.trigger('tree-wind-controller.set_wind_strength', val)
    }
    return <div>
        <p>Wind Strength:</p>
        <$Slider react={react} value={data} onValueChanged={(val) => updateData("wind_strength", val)} />
    </div>
}

const $TreeWindController = ({react}) => {
    const [data, setData] = react.useState(0)
    useDataUpdate(react, "tree-wind-controller.wind_strength", setData)

    const style = {
        height: "auto",
        width: "600rem",
    }

    return <div>
        <$Panel title="Tree Wind Controller" react={react} style={style}>
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
const $Slider = ({ react, value, onValueChanged, style }) => {
    const sliderRef = react.useRef(null);
    const [isMouseDown, setIsMouseDown] = react.useState(false);

    const updateValue = (e) => {
        const slider = sliderRef.current;
        if (!slider) return;

        const rect = slider.getBoundingClientRect();
        const position = e.clientX - rect.left;
        let newValue = (position / rect.width) * 100;
        newValue = Math.max(0, Math.min(100, Math.round(newValue)));

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

    const valuePercent = value + "%";
    return (
        <div style={{ width: '100%', ...style }}>
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center', margin: '10rem', marginTop: '0' }}>
                <div className="value_jjh" style={{ display: 'flex', width: '45rem', alignItems: 'center', justifyContent: 'center' }}>{valuePercent}</div>
                <div
                    className="slider_fKm slider_pUS horizontal slider_KjX"
                    style={{ flex: 1, margin: '10rem' }}
                    ref={sliderRef}
                    onMouseDown={handleMouseDown}
                    onMouseMove={handleMouseMove}
                    onMouseUp={handleMouseUp}>
                    <div className="track-bounds_H8_">
                        <div className="range-bounds_lNt" style={{ width: valuePercent }}>
                            <div className="range_KXa range_iUN"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}