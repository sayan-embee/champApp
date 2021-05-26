import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getAwardListAPI = async (data: any) => {
    console.log('In api',  getUrl(routers.getawardlist),data)
    return await axios.post(getUrl(routers.getawardlist),data);
}


export const getAwardListByCardAPI = async (data: any) => {
    console.log('In api',  getUrl(routers.getawardlistbycard),data)
    return await axios.post(getUrl(routers.getawardlistbycard),data);
}

export const getAwardListByRecipentAPI = async (data: any) => {
    console.log('In api',  getUrl(routers.getawardlistbyrecipent),data)
    return await axios.post(getUrl(routers.getawardlistbyrecipent),data);
}
