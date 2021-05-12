import React from 'react';
import { Flex, Text, Card, CardBody } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
// import { getBadgesAPI } from "../apis/getBadgesApi";
import { getApplauseCardAPI } from "./../apis/ApplauseCardApi"
import "./../styles.scss"

interface IProps {
    history?: any;
    location?: any
}

interface IState {
    badgeName?: any;
    Badge?: any;
    telemetry?: any;
    entityId?: any;
    teamId?: any;
    userEmail?: any;
    theme?: any;
    token?: any;
    groupId?: any;
    userId?: any;
   chatId?: any;
   personalChat?:any;
   UPN?: any;
   channelName?:any;
   authToken?:any

}

class Badges extends React.Component<IProps, IState> {

    constructor(props: IProps) {
        super(props);
        this.state = {
            // Badge: [
            //     { name: "Platinum", image: require("./../assets/platinum.png"), color:"#b4d8f5" },
            //     { name: "Gold", image: require("./../assets/gold.png") , color:"#FFF1E6"},
            //     { name: "Silver", image: require("./../assets/silver.png"), color:"#efecec" },
            //     { name: "Bronze", image: require("./../assets/bronze.svg"), color:"#f7d3bd"},
            //     { name: "Well Done", image: require("./../assets/welldone.png"), color:"#a8f38f"},
            //      { name: "Thank You", image: require("./../assets/thankyou.svg"), color:"#d08ef5" }
            // ],
        };

    }




    componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.getContext((context) => {
            console.log("context check log", context);
            this.setState({
                "teamId": context.teamId && context.teamId,
                "groupId": context.groupId && context.groupId,
                "userId": context.userObjectId && context.userObjectId,
                "UPN":context.userPrincipalName && context.userPrincipalName,
                "chatId":context.chatId && context.chatId,
                "personalChat" : context.chatId ? true: false,
                "channelName":context.channelName && context.channelName
            
            })
            console.log("context check log 1", context);
        });

        const search = window.location.search;
        const params = new URLSearchParams(search);
        this.setState({
            token: params.get("token"),
            theme: params.get("theme"),
            entityId: params.get("entityId"),
            authToken:params.get("userAuthToken") && params.get("userAuthToken")
        }, () => {
            this.getBadge()
        })
    }
    getBadge() {
        // getBadgesAPI(this.state.token).then(res => {
        //     // console.log("badge api response log", res.data)
            // const badgeData = res.data.map((e: any) => {
            //     let b = {
            //         "badgeName": e.name,
            //         "badgeImage":  e.imageUrl,
            //         "badgeColor": e.color,
            //         "badgeId": e.id
            //     }
            //     return b
            // })
            // this.setState({
            //     Badge: badgeData
            // })
        // })
        const data = {
            "CardId": 0
        }
        getApplauseCardAPI(data).then((res)=>{
            

            const badgeData = res.data.filter((e:any)=>e.isActive===1).map((e: any) => {
                let b = {
                    "badgeName": e.cardName,
                    "badgeImage":  e.cardImage,
                    "badgeColor": "#ffffff",
                    "badgeId": e.cardId
                }
                return b
            })
            this.setState({
                Badge: badgeData
            })
        })

    }





    check = (data: any) => {
        this.setState({
            badgeName: data.name,
        }, () => {
            this.props.history.push({ pathname: "/details", state: { data: data, token: this.state.token, groupId: this.state.groupId, chatId:this.state.chatId, personalChat:this.state.personalChat, userId:this.state.userId, UPN:this.state.UPN, channelName:this.state.channelName, authToken:this.state.authToken } })
        })
    }


    render() {

        return (
            <div>
                <Card fluid className="containerCard badegCardWidth">
                    <CardBody>
                        <Flex hAlign="center" vAlign="center" padding="padding.medium"><Text>Select an Applaud award</Text></Flex>
                        <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gridGap: 20 }}>
                            {this.state.Badge && this.state.Badge.map((e: any) => {
                                return <Card onClick={() => this.check(e)} ghost centered className="badgeImageCard">
                                    <img src={e.badgeImage} className="badgeImg" />
                                    <text>{e.badgeName}</text>
                                </Card>
                            })}
                        </div>
                    </CardBody>
                </Card>
            </div>
        );
    }
}



export default Badges;

