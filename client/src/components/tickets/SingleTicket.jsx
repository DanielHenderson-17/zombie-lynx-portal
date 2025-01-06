import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getTicketById } from "../../managers/ticketManager";
import { formatLongDateTime } from "../../utils/longDateTime";
import { categoryFormatter } from "../../utils/categoryFormater";
import { getGameImage } from "../../utils/gameFormatter";

export default function SingleTicket() {
  const { ticketId } = useParams();
  const [ticket, setTicket] = useState(null);

  useEffect(() => {
    const fetchTicket = async () => {
      console.log(ticket);
      try {
        const data = await getTicketById(ticketId);
        setTicket(data);
      } catch (error) {
        console.error("Error fetching ticket:", error);
      }
    };

    fetchTicket();
  }, [ticketId, ticket]);

  if (!ticket) {
    return <p>Loading ticket details...</p>;
  }

  return (
    <div className="text-white col-6 mx-auto mt-5 pt-5">
      <h2 className="text-start mb-2 fs-2 ms-1">{ticket.subject}</h2>
      <div className="d-flex justify-content-between mb-3">
        <div className="d-flex align-items-center fs-5">
          <div
            className="me-2"
            dangerouslySetInnerHTML={{
              __html: categoryFormatter(ticket.category),
            }}
          ></div>
          {ticket.category}
        </div>
        <div className="d-flex align-items-center">
          <img
            className="gameImg me-2"
            src={getGameImage(ticket.game)}
            alt=""
          />{" "}
          {ticket.server}
        </div>
      </div>
      <small className="text-start d-block mb-5" style={{ fontSize: "0.7rem" }}>
        <i className="bi bi-calendar-date me-2 ms-1"></i>
        {formatLongDateTime(ticket.updatedAt)}
      </small>
      <div className="text-start mb-5">
        <strong className="text-start">Description:</strong>{" "}
        <p>{ticket.description}</p>
      </div>
      <p className="d-flex">
        <div className="text-start me-3">Assigned Users:</div>{" "}
        {ticket.assignedUsers.map((user) => (
          <span key={ticket.id}>
            {user.firstName} {user.lastName}
            {ticket.assignedUsers.indexOf(user) !==
            ticket.assignedUsers.length - 1
              ? ", "
              : ""}
          </span>
        ))}
      </p>
    </div>
  );
}
