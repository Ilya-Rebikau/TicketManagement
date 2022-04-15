import React, {Component} from "react";
import {appsettings} from "../../appsettings"
import Cookies from 'js-cookie'

export class EventSeats extends Component{
    constructor(props){
        super(props);
        this.state = {
            eventSeats:[],
            pageNumber: 1,
            modalTitle:"",
            Id:0,
            EventAreaId:0,
            Row:0,
            number:0,
            State:""
        }
    }

    nextPage(){
        this.setState({ pageNumber: this.state.pageNumber+1});
        this.refreshList();
    }

    previousPage(){
        this.setState({ pageNumber: this.state.pageNumber-1});
        this.refreshList();
    }

    changeEventAreaId = (e) =>{
        this.setState({
            EventAreaId:e.target.value
        });
    }

    changeRow = (e) =>{
        this.setState({
            Row:e.target.value
        });
    }

    changeNumber = (e) =>{
        this.setState({
            number:e.target.value
        });
    }

    changeState = (e) =>{
        this.setState({
            State:e.target.value
        });
    }

    refreshList(){
        var url = new URL(appsettings.EventManagerApiAddress + 'eventseats/geteventseats'),
            params = {pageNumber:this.state.pageNumber}
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
        fetch(url, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({ eventSeats: data}))
    }

    componentDidMount(){
        this.refreshList();
    }

    convertState(state){
        if (state === 0){
            return 'Free'
        }
        else{
            return 'Occupied'
        }
    }

    onCreateClick(){
        fetch(appsettings.EventManagerApiAddress + 'eventseats/create',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: 0,
                EventAreaId: this.state.EventAreaId,
                Row: this.state.Row,
                Number: this.state.number,
                State: this.state.State
            })
        }).then((result)=>{
            this.refreshList();
        })
    }

    onEditClick(id){
        fetch(appsettings.EventManagerApiAddress + 'eventseats/edit/' + id,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id: id,
                EventAreaId: this.state.EventAreaId,
                Row: this.state.Row,
                Number: this.state.number,
                State: this.state.State
            })
        }).then((result)=>{
            this.refreshList();
        })
    }

    createClick(){
        this.setState({
            modalTitle:"Add event seat",
            EventAreaId:0,
            Row:0,
            number:0,
            State:"Free",
        });
    }

    editClick(eventseat){
        this.setState({
            modalTitle:"Edit event seat",
            Id:eventseat.id,
            EventAreaId:eventseat.eventAreaId,
            Row:eventseat.row,
            number:eventseat.number,
            State:this.convertState(eventseat.state),
        });
    }

    deleteClick(id){
        if(window.confirm('Are you sure?')){
            fetch(appsettings.EventManagerApiAddress + 'eventseats/delete/' + id,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json',
                    'authorization': Cookies.get('JwtTokenCookie')
            }})
            .then((result)=>{
                this.refreshList();
            })
        }
    }

    render(){
        const {
            eventSeats,
            modalTitle,
            Id,
            EventAreaId,
            Row,
            number,
            State,
        }=this.state;

        return(
            <div>
                <p>
                    <button type="button"
                        className="btn btn-primary"
                        data-bs-toggle="modal"
                        data-bs-target="#modalWindow"
                        onClick={()=>this.createClick()}>
                            Add event seat
                    </button>
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
                            <th>
                                Operations
                            </th>
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
                                    <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                                            data-bs-target="#modalWindow" onClick={()=>this.editClick(eventseat)}>
                                        Edit
                                    </button>
                                    <button type="button" className="btn btn-danger" onClick={()=>this.deleteClick(eventseat.id)}>
                                        Delete
                                    </button>
                                </td>
                            </tr>)}
                    </tbody>
                </table>

                <div>
                    {this.state.pageNumber > 1?
                        <button type="button" className="btn btn-primary" onClick={() => this.previousPage()}>&lt;</button>:null}
                    <button type="button" className="btn btn-primary" onClick={() => this.nextPage()}>&gt;</button>
                    
                </div>

                <div className="modal fade" id="modalWindow" tabIndex="-1" aria-hidden="true">
                    <div className="modal-dialog modal-lg modal-dialog-centered">
                        <div className="modal-content">
                            <div className="modal-header">
                                <h5 className="modal-title">{modalTitle}</h5>
                                <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"/>
                            </div>

                            <div className="modal-body">
                                <div className="input-group mb-3">
                                    <span className="input-group-text">EventAreaId</span>
                                    <input type="number" className="form-control" value={EventAreaId} onChange={this.changeEventAreaId}/>
                                    <span className="input-group-text">Row</span>
                                    <input type="number" className="form-control" value={Row} onChange={this.changeRow}/>
                                    <span className="input-group-text">Number</span>
                                    <input type="number" className="form-control" value={number} onChange={this.changeNumber}/>
                                    <span className="input-group-text">State</span>
                                    <input type="text" className="form-control" value={State} onChange={this.changeState}/>
                                </div>
                                {Id===0?
                                <button type="button" className="btn btn-primary float-start"onClick={()=>this.onCreateClick()}>
                                    Create</button>:null}
                                {Id!==0?
                                <button type="button" className="btn btn-primary float-start" onClick={()=>this.onEditClick(Id)}>
                                    Update</button>:null}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        )
    }
}