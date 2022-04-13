import React, {Component} from "react";
import {appsettings} from "../../appsettings"
import Cookies from 'js-cookie'
import axios from 'axios';
import {NavLink} from 'react-router-dom';

export class EventSeats extends Component{
    constructor(props){
        super(props);
        this.state = {
            eventSeats:[],
            pageNumber: 1,
        }
    }

    componentDidMount(){
        axios.get(appsettings.EventManagerApiAddress + 'eventseats/geteventseats', {
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
              },
            params: {
                'pageNumber': this.state.pageNumber
            }
        })
        .then(res => {
        const eventSeats = res.data;
        this.setState({ eventSeats });
      })
    }

    convertState(state){
        if (state === 0){
            return 'Free'
        }
        else{
            return 'Occupied'
        }
    }

    render(){
        const {
            eventSeats
        }=this.state;

        return(
            <div>
                <p>
                    <NavLink className="btn btn-primary" to="/eventseats/create">Create</NavLink>
                </p>
                <table className="table">
                    <thead>
                        <tr>
                            <th>
                                EventAreaId
                            </th>
                            <th>
                                Row
                            </th>
                            <th>
                                Number
                            </th>
                            <th>
                                State
                            </th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {eventSeats.map(eventseat => 
                            <tr key={eventseat.id}>
                                <td>
                                    {eventseat.eventAreaId}
                                </td>
                                <td>
                                    {eventseat.row}
                                </td>
                                <td>
                                    {eventseat.number}
                                </td>
                                <td>
                                    {this.convertState(eventseat.state)}
                                </td>
                                <td>
                                    {/* <a className="btn btn-primary" asp-action="Edit" asp-route-id="@item.Id">@loc["Edit"]</a>
                                    <a className="btn btn-primary" asp-action="Details" asp-route-id="@item.Id">@loc["Details"]</a>
                                    <a className="btn btn-danger" asp-action="Delete" asp-route-id="@item.Id">@loc["Delete"]</a> */}
                                </td>
                            </tr>)}
                    </tbody>
                </table>
            </div>
        )
    }
}