import React from "react";

const ModalButton = ({ btnTitle, ...props }) => {
    return (
        <div>
            <button {...props} type="button" className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exampleModal">
                {btnTitle}
            </button>
        </div>
    );
};

export default ModalButton;