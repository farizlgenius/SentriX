export interface CommandLine{
      text:string;
      color:LineColor
}

export interface LineColor{
      name:LineColorConst;
}

export enum LineColorConst{
      Red="Red",
      Black="Black",
      white="White",
      Green="Green"
}