CREATE TABLE Especialidad (
	idEspecialidad int identity(1,1) not null primary key,
	nombreEspecialidad varchar(55) not null
)

CREATE TABLE Paciente (
    dniPaciente VARCHAR(8) not null PRIMARY KEY,
    nombrePaciente VARCHAR(150) not null,
    emailPaciente VARCHAR(55) not null,
    telefonoPaciente VARCHAR(25) not null
);

CREATE TABLE Profesional (
	idProfesional int identity (1,1) not null primary key,
	nombreProfesional varchar(150) not null,
	emailProfesional varchar (55) not null,
	telefonoProfesional varchar (25) not null,
	idEspecialidad int not null,
	FOREIGN KEY (idEspecialidad) REFERENCES Especialidad(idEspecialidad)
	);


CREATE TABLE Disponibilidad (
    idDisponibilidad INT IDENTITY(1,1) not null PRIMARY KEY,      
    horaInicio TIME not null,                               
    horaFin TIME not null,                                  
    idProfesional INT not null,                             
    dia VARCHAR(10)not null,                                
    estado VARCHAR(12)not null,
    CONSTRAINT CHK_Estado CHECK (estado IN ('Disponible', 'Ocupado')),                             
    CONSTRAINT FK_Profesional FOREIGN KEY (idProfesional) REFERENCES Profesional(idProfesional)
);

CREATE TABLE Turno (
    idTurno INT PRIMARY KEY,
    fechaCreacion DATE NOT NULL,
    horaCreacion TIME(0) NOT NULL, -- sin decimales en los segundos
    idEspecialidad INT NOT NULL,
    idProfesional INT NOT NULL,
    dniPaciente  VARCHAR(8)NOT NULL,
    idDisponibilidad INT NOT NULL,
    estadoTurno VARCHAR(12) NOT NULL,
	CONSTRAINT CHK_EstadoTurno CHECK (estadoTurno IN ('Confirmado', 'Pendiente', 'Cancelado')),
    CONSTRAINT FK_Especialidad FOREIGN KEY (idEspecialidad) REFERENCES Especialidad(idEspecialidad),
    CONSTRAINT FK_ProfesionalTurno FOREIGN KEY (idProfesional) REFERENCES Profesional(idProfesional),
    CONSTRAINT FK_Paciente FOREIGN KEY (dniPaciente) REFERENCES Paciente(dniPaciente),
    CONSTRAINT FK_Disponibilidad FOREIGN KEY (idDisponibilidad) REFERENCES Disponibilidad(idDisponibilidad)
);

Select * from Especialidad
