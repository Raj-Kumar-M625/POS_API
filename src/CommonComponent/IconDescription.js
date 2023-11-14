import React from 'react';
import './../SCSS/iconDescription.css';


export const IconDescription = ({ icon, name, description }) => {
    return (
        <div className="widgets_div">
            <div className='icon_div'>
                <div className={[icon, "iconCls"].join(' ')}></div>
            </div>
            <div>
                <div  className='name_div'>{name}</div>
                <div  className='desc_div'>{description}</div>
            </div>
        </div>
    )
}