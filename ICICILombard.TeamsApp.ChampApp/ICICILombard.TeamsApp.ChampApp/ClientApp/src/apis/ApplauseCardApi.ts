import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getApplauseCardAPI = async (data: any) => {
    console.log('In api',  getUrl(routers.getapplausecard),data)
    return await axios.post(getUrl(routers.getapplausecard),data);
}


export const addApplauseCardAPI = async (data: any) => {
    console.log('In api',  getUrl(routers.addapplausecard),data)
    return await axios.post(getUrl(routers.addapplausecard),data);
}
