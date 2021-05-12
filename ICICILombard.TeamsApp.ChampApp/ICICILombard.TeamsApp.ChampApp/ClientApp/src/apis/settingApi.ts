import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getAppSettingAPI = async () => {
    console.log('In api',  getUrl(routers.getappsetting))
    return await axios.get(getUrl(routers.getappsetting));
}


export const updateAppSettingAPI = async (data:any) => {
    console.log('In api',  getUrl(routers.updateappsetting),data)
    return await axios.post(getUrl(routers.updateappsetting),data);
}
