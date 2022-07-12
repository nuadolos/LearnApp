import React from "react";

const Select = ({ options, defaultValue, ...props }) => {
    return (
        <select className="form-select" {...props}>
            <option disabled value="">{defaultValue}</option>
            {options.map(option =>
                <option key={option.id} value={option.id}>{option.name}</option>
            )}
        </select>
    );
};

export default Select;