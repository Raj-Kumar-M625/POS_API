//import { privateAxios } from "../context/Axios";
import SurveyService from "./SurveyService";
// const Axios = require("../context/Axios");
// const privateAxios = Axios.privateAxios;
import axios from "axios";
const StorageFactory = { StoreData, UpdateData, ExtractData, postMapperInfo };

async function StoreData(data, multiselect, surveyId, config, QId, User) {
  const SurveyDtoList = [];
  const url = `${config.localUrl}Survey/SaveSurveyDetailAB?surveyId=${surveyId}`;
  debugger;
  for (var json in data) {
   
    SurveyDtoList.push({
      questionId: parseInt(json.match(/(\d+)/)),
      answer: data[json] === null ? "" : ( data[json] !== "" ? data[json]?.replace(/[<>]/gi, '') : ""),
      surveyId: surveyId,
      QuestionPaperId: QId,
      userId: User !== null ? User.userId.toString() : "",
    });
  }

  if (Object.keys(multiselect).length > 0) {

    for (const key in multiselect) {
      SurveyDtoList.push({
        questionId: parseInt(key.match(/(\d+)/)),
        answer: multiselect[key],
        surveyId: surveyId,
        QuestionPaperId: QId,
        userId: User !== null ? User.userId.toString() : ""
      });

    }
  }
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });
  debugger
  await privateAxios.post(url, SurveyDtoList).then((res) => {
    debugger
    console.log(res);
    return res;
  }).catch((error) => {
    console.log(error);
  });
}

async function postMapperInfo(config, surveyMapper) {
  const privateAxios = axios.create({
    baseURL: config.localUrl,
    headers: {
      "Authorization": `Bearer ${sessionStorage.getItem("token")}`
    },

  });
  debugger;
  let uri = `${config.localUrl}SurveyMapper/GetMappedSurveyByID?Surveyid=${surveyMapper.surveyId}`;
  const mapperdata = await privateAxios.get(uri);
  if (mapperdata.data.findIndex(item => item.categoryId === surveyMapper.CategoryId) < 0) {
    const mapperUrl = `${config.localUrl}SurveyMapper/MapSurvey`;
    await privateAxios.post(mapperUrl, surveyMapper).then((res) => {
      debugger
      console.log(res)
    }).catch((error) => {
      debugger;
      console.log(error);
    });
  }

}

async function UpdateData(data, multiselect, SurveyId) {
  const updateAnswer = [];

  for (var json in data) {
    if (data[json] !== undefined && data[json].length > 0) {
      updateAnswer.push({
        answer: data[json] === null ? "" : data[json],
        questionId: parseInt(json.match(/(\d+)/)),
        surveyId: SurveyId,
      });
    }
  }
  console.log(multiselect)
  if (Object.keys(multiselect).length > 0) {
    for (const key in multiselect) {
      updateAnswer.push({
        questionId: parseInt(key.match(/(\d+)/)),
        answer: multiselect[key],
        surveyId: SurveyId,
      });

    }
  }
  SurveyService.updateSurvey(updateAnswer, SurveyId);
}

function ExtractData(SurveyId, PersonalDetails) {
  console.log(SurveyId);
  const data = SurveyService.GetSurveyById(SurveyId);

  var extractInfo = {};
  const extractInfo_arr = [];
  if (data.length !== 0) {
    for (let i = 0; i < PersonalDetails.QuestionsPage2.length; i++) {
      var bool = false;
      for (let j = 0; j < data.length; j++) {
        if (
          PersonalDetails.QuestionsPage2[i].QuestionId === data[j].questionId
        ) {
          extractInfo.QuestionId = PersonalDetails.QuestionsPage2[i].QuestionId;
          extractInfo.Answer = data[j].answer;
          extractInfo.QText = PersonalDetails.QuestionsPage2[i].QText;
          extractInfo.AnswerChoices =
            PersonalDetails.QuestionsPage2[i].AnswerChoices;
          extractInfo.QuestionTypeName =
            PersonalDetails.QuestionsPage2[i].QuestionTypeName;
          bool = true;
        }
      }

      if (bool === false) {
        extractInfo.QuestionId = PersonalDetails.QuestionsPage2[i].QuestionId;
        extractInfo.AnswerChoices =
          PersonalDetails.QuestionsPage2[i].AnswerChoices;

        extractInfo.Answer = null;
        extractInfo.QText = PersonalDetails.QuestionsPage2[i].QText;
        extractInfo.QuestionTypeName =
          PersonalDetails.QuestionsPage2[i].QuestionTypeName;
      } else {
        bool = false;
      }
      extractInfo_arr.push(extractInfo);
      extractInfo = {};
    }
  }

  return extractInfo_arr;
}

export default StorageFactory;
