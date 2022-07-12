import React from "react";

const Loader = () => {
    return (
        <div className="container" style={{display: 'flex', justifyContent: 'center'}}>
            <div className="spinner-border" style={{ width: '200px', height: '200px' }} role="status"></div>
        </div>
    );
}

export default Loader;