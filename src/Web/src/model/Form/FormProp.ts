export enum FormType {
    CREATE ,
    INFO ,
    UPDATE 
}

export interface FormProp<T>{
    type: FormType,
    handleClick?: (e: React.MouseEvent<HTMLButtonElement>) => void,
    handleChange:(e: React.ChangeEvent<HTMLSelectElement | HTMLInputElement>) => void,
    setDto: React.Dispatch<React.SetStateAction<T>>;
    dto: T,
    setIsNext?:React.Dispatch<React.SetStateAction<boolean>>;
}