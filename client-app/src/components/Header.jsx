import "../styles/Header.css";
import Image from '../assets/img/nipi_logo.png';

export default function Header(){

    return (
        <div className='header'>
            <nav className="neu navbar navbar-expand-md mb-4">
            <div className="container-fluid">
                <img src={Image} alt="logo" width="35" height="35"></img>
                <a>  </a>
                <button className="neu navbar-toggler" type="button" data-bs-toggle="collapse"
                        data-bs-target="#navbarCollapse"
                        aria-controls="navbarCollapse" aria-expanded="false" aria-label="Переключить навигацию">
                        <span className="navbar-toggler-icon">
                            <img alt='' draggable="false" src="../assets/svg/list.svg"/>
                        </span>
                </button>
                <div className="collapse navbar-collapse" id="navbarCollapse">

                    <ul className="navbar-nav mb-2 mb-md-0">
                        <li className="nav-item">
                            <a className="navbar-brand" aria-current="page" href="#">Менеджер файлов</a>
                        </li>
                    </ul>
                    <div className="d-flex ms-auto" style={{marginRight:'30px'}}>
                    <form class="col-12 col-lg-auto mb-3 mb-lg-0">
                     <input type="search" class="form-control" placeholder="Search..." aria-label="Search"/>
                    </form>
                    </div>
                </div>
            </div>
        </nav>
        </div>
        
    )
}