import React from 'react'

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

export { $Slider }