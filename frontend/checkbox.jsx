import React from 'react'

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
                <div style={{}}>
                    {displayLabel}
                </div>
                <div className={many('toggle_cca toggle_ATa', checked_class)} style={{ marginRight: '10rem' }} onClick={handleClick}>
                    <div className={many('checkmark_NXV', checked_class)}></div>
                </div>
            </div>
        </div>
    );
}

export { $CheckBox }