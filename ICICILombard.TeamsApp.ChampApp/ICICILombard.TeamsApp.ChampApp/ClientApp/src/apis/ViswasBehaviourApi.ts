import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getViswasBehaviourAPI = async (data:any) => {
    console.log('In api',  getUrl(routers.getbehaviours),data)
    return await axios.post(getUrl(routers.getbehaviours),data);
}


export const addViswasBehaviourAPI = async (data:any) => {
    console.log('In api',  getUrl(routers.addbehaviours),data)
    return await axios.post(getUrl(routers.addbehaviours),data);
}
