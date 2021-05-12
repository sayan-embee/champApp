import axios from 'axios';
import {getUrl,routers} from './Endpoint'

export const getTeamMembersAPI = async (teamId: string, token: any): Promise<any> => {
    console.log("team member url",getUrl(routers.teammember)+teamId,"token",token); 
    return await axios.get(getUrl(routers.teammember)+teamId,{headers: {"Authorization" : "Bearer "+ token}})
}

export const getChatMembersAPI = async (chatId: string, token: any): Promise<any> => {
    console.log("team member url",getUrl(routers.chatmember)+chatId,"token",token); 
    return await axios.get(getUrl(routers.chatmember)+chatId,{headers: {"Authorization" : "Bearer "+ token}})
}