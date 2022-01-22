import axios, { AxiosResponse } from 'axios';
import { AttributesModel } from '../models/AttributesModel';
import { DocumentAttributesModel } from '../models/DocumentAttributesModel';
import { FileModel } from '../models/FileModel';

const sleep = (delay: number) => 
{
    return new Promise((resolve)=>
    {
        setTimeout(resolve, delay)
    })
}

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.response.use(async response =>{
   try{
       await sleep(1000);
       return response;
   } catch(error){
       console.log(error);
       return await Promise.reject(error);
   }
})

const responseBody = <T> (response: AxiosResponse<T>) => response.data;

const requests = {
  get: <T>  (url: string) => axios.get<T>(url).then(responseBody),
  post: <T>  (url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
  put: <T>  (url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
  delete: <T> (url: string) => axios.delete<T>(url).then(responseBody)
}

const Files = {
    list: () => requests.get<FileModel[]>('/UploadFiles'),
    element: (fileId: number) => requests.get<AttributesModel>(`/UploadFiles/${fileId}`),
    upload: (file: FormData) => requests.post<void>(`/UploadFiles`, file)

}

const Attributes = {
    list: () => requests.get<AttributesModel[]>('/Attributes'),
    element: (fileId: number) => requests.get<AttributesModel>(`/Attributes/${fileId}`),
    // save:() => axios.post<void>(`/Attributes`),
    saveElement:(fileId: number) => axios.post<void>(`/Attributes/${fileId}`)
}
const Svg = {
    svg: (fileId: number) => requests.get<any>(`/Svg/${fileId}`)
}

const formData = new FormData()

const agent = {
    Files,
    Attributes,
    Svg
}

export default agent;