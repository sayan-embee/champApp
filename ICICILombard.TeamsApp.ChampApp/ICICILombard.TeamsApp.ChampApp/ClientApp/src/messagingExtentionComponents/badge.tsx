import React from 'react';
import { Flex, Text, Card, CardBody, Loader } from "@fluentui/react-northstar";
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
   UPN?: any;
   channelName?:any;
   loading?: any;
    channelId?: any;
    userObjectId?:any;
    selectedBadgeId?:any;
    teamName?:any;
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
            loading:true
        };

    }




    componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.getContext((context) => {
            console.log("context check log", context);
            this.setState({
                "teamId": context.teamId && context.teamId,
                "groupId": context.groupId && context.groupId,
                "channelId": context.channelId && context.channelId,
                "userId": context.userObjectId && context.userObjectId,
                "UPN":context.userPrincipalName && context.userPrincipalName,
                "chatId":context.chatId && context.chatId,
                "channelName":context.channelName && context.channelName,
                "teamName":context.teamName && context.teamName,
                "userObjectId":context.userObjectId && context.userObjectId
            
            })
        });

        const search = window.location.search;
        const params = new URLSearchParams(search);
        this.setState({
            token: params.get("token"),
            theme: params.get("theme"),
            entityId: params.get("entityId"),
            selectedBadgeId:params.get("badgeId")
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
                Badge: badgeData,
                loading:false
            })
        })

    }





    check = (data: any) => {
        this.setState({
            badgeName: data.name,
        }, () => {
            this.props.history.push({ pathname: "/details", state: { data: data, token: this.state.token, groupId: this.state.groupId, chatId:this.state.chatId,  userId:this.state.userId, UPN:this.state.UPN, channelName:this.state.channelName, teamId:this.state.teamId, channelId:this.state.channelId, userObjectId:this.state.userObjectId, teamName:this.state.teamName } })
        })
    }


    render() {
       return (
            <div>
                <Card fluid className="containerCard badegCardWidth">
                {!this.state.loading?<CardBody styles={{marginTop:"20px"}}>
                        <Flex hAlign="center" vAlign="center" padding="padding.medium"><Text>Choose a card</Text></Flex>
                       <div style={{ display: "grid", gridTemplateColumns: "repeat(3, 1fr)", gridGap: 20 }} className="badgeImageCardGridDiv">
                            {this.state.Badge && this.state.Badge.map((e: any) => {
                                return <div className={`badgeImageCard ${(this.state.selectedBadgeId === e.badgeId) && 'selectedBadge' } `}><Card onClick={() => this.check(e)} ghost centered className={`badgeImageCard ${(this.state.selectedBadgeId === e.badgeId) && 'selectedBadge' } `}>
                                    <img src={e.badgeImage} className="badgeImg" />
                                    <text>{e.badgeName}</text>
                                </Card></div>
                            })}
                        </div>
                    </CardBody>:<Loader styles={{ margin: "50px" }} />}
                </Card>
            </div>
        );
    }
}



export default Badges;

