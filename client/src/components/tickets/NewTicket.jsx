import { useEffect, useState } from "react";
import { createTicket, getTicketOptions } from "../../managers/ticketManager";
import { getAllUsers } from "../../managers/userProfileManager";

export default function NewTicket() {
  // State to store options and form data
  const [options, setOptions] = useState({
    games: [],
    servers: [],
    categories: [],
    users: [],
  });
  const [formData, setFormData] = useState({
    subject: "",
    category: "",
    game: "",
    server: "",
    description: "",
    assignedUserIds: [],
  });
  const [loading, setLoading] = useState(true);

  // Fetch ticket options and users
  useEffect(() => {
    const fetchOptions = async () => {
      try {
        const ticketOptions = await getTicketOptions();
        const users = await getAllUsers();

        setOptions({
          games: ticketOptions.games,
          servers: ticketOptions.servers,
          categories: ticketOptions.categories,
          users: users.map((user) => ({
            id: user.id,
            name: `${user.firstName} ${user.lastName}`,
          })),
        });
        setLoading(false);
      } catch (error) {
        console.error("Error fetching options or users:", error);
        setLoading(false);
      }
    };

    fetchOptions();
  }, []);

  // Handle form input changes
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await createTicket(formData);
      alert("Ticket created successfully!");
      setFormData({
        subject: "",
        category: "",
        game: "",
        server: "",
        description: "",
        assignedUserIds: [],
      });
    } catch (error) {
      console.error("Error creating ticket:", error);
    }
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  return (
    <div className="new-ticket-form">
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label htmlFor="subject">Subject</label>
          <input
            type="text"
            id="subject"
            name="subject"
            value={formData.subject}
            onChange={handleChange}
            required
          />
        </div>

        <div className="mb-3">
          <label htmlFor="category">Category</label>
          <select
            id="category"
            name="category"
            value={formData.category}
            onChange={handleChange}
            required
          >
            <option value="">Select a category</option>
            {options.categories.map((category, index) => (
              <option key={index} value={category}>
                {category}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label htmlFor="game">Game</label>
          <select
            id="game"
            name="game"
            value={formData.game}
            onChange={handleChange}
            required
          >
            <option value="">Select a game</option>
            {options.games.map((game, index) => (
              <option key={index} value={game}>
                {game}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label htmlFor="server">Server</label>
          <select
            id="server"
            name="server"
            value={formData.server}
            onChange={handleChange}
            required
          >
            <option value="">Select a server</option>
            {options.servers.map((server, index) => (
              <option key={index} value={server}>
                {server}
              </option>
            ))}
          </select>
        </div>

        <div className="mb-3">
          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleChange}
            required
          />
        </div>

        <button type="submit">Create Ticket</button>
      </form>
    </div>
  );
}
