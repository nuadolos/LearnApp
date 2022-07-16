import React from "react";

const ModalLink = (props) => {
    return (
        <div>
            <svg {...props} xmlns="http://www.w3.org/2000/svg" enableBackground="new 0 0 91 91" id="Layer_1" version="1.1" viewBox="0 0 91 91" width="20px"
                data-bs-toggle="modal" data-bs-target="#exampleModal" style={{ cursor: 'pointer' }}>
                <g>
                    <g>
                        <path d="M24.018,49.415L42.149,67.54l38.119-38.122L62.145,11.292L24.018,49.415z M66.444,25.118    c1.259,1.262,1.259,3.306,0,4.566L44.369,51.753c-0.629,0.633-1.455,0.946-2.278,0.946c-0.831,0-1.655-0.313-2.284-0.946    c-1.261-1.262-1.261-3.305,0-4.565l22.072-22.07C63.138,23.857,65.183,23.857,66.444,25.118z" fill="#6EC4A7" />
                        <polygon fill="#65B794" points="15.72,75.578 35.863,70.393 21.111,55.641   " />
                        <path d="M87.621,12.533l-8.593-8.596c-2.548-2.546-6.992-2.543-9.53,0L66.706,6.73l18.129,18.124l2.786-2.788    C90.244,19.436,90.244,15.159,87.621,12.533z" fill="#65B794" />
                    </g>
                    <path d="M79.585,89.554H3.461c-1.833,0-3.318-1.486-3.318-3.319V5.512c0-1.833,1.484-3.318,3.318-3.318h51.113   c1.835,0,3.317,1.485,3.317,3.318c0,1.832-1.482,3.318-3.317,3.318H6.779v74.086h72.806c2.592,0,4.701-2.108,4.701-4.701V36.588   c0-1.833,1.484-3.318,3.317-3.318c1.832,0,3.318,1.485,3.318,3.318v41.627C90.922,84.467,85.836,89.554,79.585,89.554z" fill="#647F94" />
                </g>
            </svg>
        </div>
    );
};

export default ModalLink;