import React from 'react'
import "../CSS/CommonComponentsCSS/Accordian.css"

function Accordian({title,children}) {
  return (
    <div className="accordion border-none m-3">
    <div className="accordion-item accordianHeader">
    
        <button className="btn accordianButton">
          {title}
        </button>
 
      <div id="collapseOne" className="accordion-collapse collapse show accordianChildren" data-bs-parent="#accordionExample">
        <div className="accordion-body">
         {children}
        </div>
      </div>
    </div>
  </div>
  )
}

export default Accordian