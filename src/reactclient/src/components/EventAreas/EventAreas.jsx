import React, {Component} from "react";
import {appsettings} from "../../appsettings"
import Cookies from 'js-cookie'

export class EventAreas extends Component{
    constructor(props){
        super(props);
        this.state = {
            eventAreas:[],
            modalTitle:"",
            Id:0,
            EventId:0,
            Description:"",
            CoordX:0,
            CoordY:0,
            Price:0,
            nextEventAreas:[]
        }
    }

    static pageNumber = 1;

    nextPageExist(){
        var nextPageNumber = EventAreas.pageNumber;
        nextPageNumber++;
        var urlNext = new URL(appsettings.EventManagerApiAddress + 'eventareas/getareas'),
        paramsNext = {pageNumber:nextPageNumber}
        Object.keys(paramsNext).forEach(key => urlNext.searchParams.append(key, paramsNext[key]))
        fetch(urlNext, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({ nextEventAreas: data }));
    }

    nextPage(){
        EventAreas.pageNumber++;
        this.refreshList();
    }

    firstPage(){
        EventAreas.pageNumber = 1;
        this.refreshList();
    }

    previousPage(){
        EventAreas.pageNumber--;
        this.refreshList();
    }

    changeEventId = (e) =>{
        this.setState({
            EventId:e.target.value
        });
    }

    changeDescription = (e) =>{
        this.setState({
            Description:e.target.value
        });
    }

    changeCoordX = (e) =>{
        this.setState({
            CoordX:e.target.value
        });
    }

    changeCoordY = (e) =>{
        this.setState({
            CoordY:e.target.value
        });
    }

    changePrice = (e) =>{
        this.setState({
            Price:e.target.value
        });
    }

    refreshList(){
        var url = new URL(appsettings.EventManagerApiAddress + 'eventareas/getareas'),
            params = {pageNumber:EventAreas.pageNumber}
        Object.keys(params).forEach(key => url.searchParams.append(key, params[key]))
        fetch(url, {
            method:"GET",
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            }
        }).then(response => response.json()).then(data => this.setState({ eventAreas: data}));
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
        fetch(appsettings.EventManagerApiAddress + 'eventareas/create',{
            method:'POST',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id:0,
                EventId:this.state.EventId,
                Description:this.state.Description,
                CoordX:this.state.CoordX,
                CoordY:this.state.CoordY,
                Price:this.state.Price
            })
        }).then(()=>{
            this.refreshList();
        })
    }

    onEditClick(id){
        fetch(appsettings.EventManagerApiAddress + 'eventareas/edit/' + id,{
            method:'PUT',
            headers:{
                'Accept':'application/json',
                'Content-Type':'application/json',
                'authorization': Cookies.get('JwtTokenCookie')
            },
            body:JSON.stringify({
                Id:id,
                EventId:this.state.EventId,
                Description:this.state.Description,
                CoordX:this.state.CoordX,
                CoordY:this.state.CoordY,
                Price:this.state.Price
            })
        }).then(()=>{
            this.refreshList();
        })
    }

    createClick(){
        this.setState({
            modalTitle:"Add event area",
            EventId:0,
            Description:"",
            CoordX:0,
            CoordY:0,
            Price:0
        });
    }

    editClick(eventarea){
        this.setState({
            modalTitle:"Edit event area",
            Id:eventarea.id,
            EventId:eventarea.eventId,
            Description:eventarea.description,
            CoordX:eventarea.coordX,
            CoordY:eventarea.coordY,
            Price:eventarea.price
        });
    }

    deleteClick(id){
        if(window.confirm('Are you sure?')){
            fetch(appsettings.EventManagerApiAddress + 'eventareas/delete/' + id,{
                method:'DELETE',
                headers:{
                    'Accept':'application/json',
                    'Content-Type':'application/json',
                    'authorization': Cookies.get('JwtTokenCookie')
            }})
            .then(()=>{
                this.refreshList();
            })
        }
    }

    render(){
        const {
            eventAreas,
            modalTitle,
            Id,
            EventId,
            Description,
            CoordX,
            CoordY,
            Price,
            nextEventAreas
        }=this.state;

        return(
            <div>
                <p>
                    <button type="button"
                        className="btn btn-primary"
                        data-bs-toggle="modal"
                        data-bs-target="#modalWindow"
                        onClick={()=>this.createClick()}>
                            Add event area
                    </button>
                </p>
                <table className="table">
                    <thead>
                        <tr>
                            <th>
                                EventId
                            </th>
                            <th>
                                Description
                            </th>
                            <th>
                                CoordX
                            </th>
                            <th>
                                CoordY
                            </th>
                            <th>
                                Price
                            </th>
                            <th>
                                Operations
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        {eventAreas.map(eventarea => 
                            <tr key={eventarea.id}>
                                <td>
                                    {eventarea.eventId}
                                </td>
                                <td>
                                    {eventarea.description}
                                </td>
                                <td>
                                    {eventarea.coordX}
                                </td>
                                <td>
                                    {eventarea.coordY}
                                </td>
                                <td>
                                    {eventarea.price}
                                </td>
                                    <td>
                                    <button type="button" className="btn btn-primary" data-bs-toggle="modal"
                                            data-bs-target="#modalWindow" onClick={()=>this.editClick(eventarea)}>
                                        Edit
                                    </button>
                                    <button type="button" className="btn btn-danger" onClick={()=>this.deleteClick(eventarea.id)}>
                                        Delete
                                    </button>
                                </td>
                            </tr>)}
                    </tbody>
                </table>

                <div>
                    {EventAreas.pageNumber!==1?
                        <button type="button" className="btn btn-primary" onClick={() => this.firstPage()}>1</button>:null}
                    {EventAreas.pageNumber > 1?
                        <button type="button" className="btn btn-primary" onClick={() => this.previousPage()}>&lt;</button>:null}
                        {console.log(this.nextPageExist())}
                    {nextEventAreas.length !== 0?
                        <button type="button" className="btn btn-primary" onClick={() => this.nextPage()}>&gt;</button>:null}
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
                                    <span className="input-group-text">EventId</span>
                                    <input type="number" className="form-control" value={EventId} onChange={this.changeEventId}/>
                                    <span className="input-group-text">Description</span>
                                    <input type="text" className="form-control" value={Description} onChange={this.changeDescription}/>
                                    <span className="input-group-text">CoordX</span>
                                    <input type="number" className="form-control" value={CoordX} onChange={this.changeCoordX}/>
                                    <span className="input-group-text">CoordY</span>
                                    <input type="number" className="form-control" value={CoordY} onChange={this.changeCoordY}/>
                                    <span className="input-group-text">Price</span>
                                    <input type="number" className="form-control" value={Price} onChange={this.changePrice}/>
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