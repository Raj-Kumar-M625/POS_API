export interface AnswerResponse{
    QuestionPaperId:number;
    QuestionPaperQuestionId:number;
    AnswerText:any;
    AdditionalComment:boolean;
    QText:string;
    QuestionTypeName:string,
    AnswerChoices:string,
    SubCategoryDesc?:string,
    SubCategoryName?: string,
    IsMandatory:boolean,
    AdditionalText:string,
}