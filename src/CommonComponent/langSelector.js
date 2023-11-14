import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import ToggleButton from '@mui/material/ToggleButton';
import ToggleButtonGroup from '@mui/material/ToggleButtonGroup';
import { styled } from "@mui/material/styles";
import MuiToggleButton from "@mui/material/ToggleButton";
import "../CSS/Views/Topbar.css"


const LangSelector = () => {


  const { i18n } = useTranslation();
  const [selectedLang, setSelectedLang] = useState(['en','kn'].includes(i18n.language) ? i18n.language : 'en');
  
  
  const changeLanguage = (event) => {
    window.location.reload()
    sessionStorage.setItem("language", event.target.value);
    setSelectedLang(event.target.value);
    i18n.changeLanguage(event.target.value);
  }

  const ToggleButton = styled(MuiToggleButton)(({ selectedColor }) => ({
  "&.Mui-selected, &.Mui-selected:hover": {
    color: "white",
    backgroundColor: selectedColor

  }
}));
  
  return (
    <ToggleButtonGroup
      orientation="vertical"
      // sx={{width:'80%'}}
      value={selectedLang}
      exclusive
      onChange={changeLanguage}
      className="language"
    >
      <ToggleButton className='selectLanguage' size='small' value="kn" aria-label="kn"   selectedColor="red"  >ಕನ್ನಡ</ToggleButton>
      <ToggleButton className='selectLanguage' size='small' value="en" aria-label="en"  selectedColor="red" > English</ToggleButton>
    </ToggleButtonGroup>
  )
}

export default LangSelector;