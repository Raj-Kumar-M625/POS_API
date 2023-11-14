import React from 'react';

export const FormInputSelect = ({ QText, val }) => {

    return (
        <>
            {/* <div class="info"> */}
            <label>{QText}</label>
            <select className='formselect'>
                <option>select</option>
                {val.map(e => <option>{e}</option>)}
            </select>
            {/* </div> */}
        </>
    )
}